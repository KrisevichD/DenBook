<?php /** @noinspection PhpUndefinedClassInspection */

use App\Dialog;
use App\Helpers\Cast;
use App\Helpers\Media;
use App\Helpers\Query;
use App\User;
use App\Video;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Http\Request;
use Intervention\Image\Facades\Image;

/*
|--------------------------------------------------------------------------
| API Routes
|--------------------------------------------------------------------------
|
| Here is where you can register API routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| is assigned the "api" middleware group. Enjoy building your API!
|
*/

const REGISTER_CODE_SUCCESS = 0;

const REGISTER_CODE_NAME_EXISTS = 1;

const REGISTER_CODE_INVALID_FIELDS = 2;

const LOGIN_CODE_NAME_NOT_EXISTS = 1;

const LOGIN_CODE_PASSWORD_NOT_MATCH = 2;

const USERS_CODE_NO_USER = '-';


Route::middleware('auth:api')->get('/user', function (Request $request) {
    return $request->user();
});


// ---------------- REGISTER/LOGIN ---------------------------

Route::post('register', function (Request $request) {
    $name     = $request->name;
    $password = $request->password;

    if (!$name || !$password) {
        return REGISTER_CODE_INVALID_FIELDS;
    }

    // TODO: convert to transaction
    if (User::whereName($name)->exists()) {
        return REGISTER_CODE_NAME_EXISTS;
    }
    try {
        return User::create([
            'name'     => $name,
            'password' => $password
        ]);
    } catch (\PDOException $e) {
        return REGISTER_CODE_NAME_EXISTS;
    }
});

Route::get('login', function (Request $request) {
    $name     = $request->name;
    $password = $request->password;

    $user = User::whereName($name)->first();
    if (!$user) {
        return LOGIN_CODE_NAME_NOT_EXISTS;
    }

    if ($user->password !== $password) {
        return LOGIN_CODE_PASSWORD_NOT_MATCH;
    }

    return $user;
});


// ---------------- USERS -------------------------------

Route::get('users', function (Request $request) {
    $userId = Cast::toMaybeInt($request->user_id);

    if ($userId) {
        $user = User::find($userId);
        return $user ?: USERS_CODE_NO_USER;
    }

    return User::all();
});

Route::post('users/{id}', function (Request $request, int $id) {
    $imageFile = $request->file('image');
    $user      = User::find($id);
    if (!$user) {
        return USERS_CODE_NO_USER;
    }

    $imagePath                 = "img/u-$id.{$imageFile->getClientOriginalExtension()}";
    $imageAbsolutePath         = Media::getAbsolutePath($imagePath);
    $originalImagePath         = "img/u-$id-orig.{$imageFile->getClientOriginalExtension()}";
    $originalImageAbsolutePath = Media::getAbsolutePath($originalImagePath);

    Storage::put(Media::getAppStorageRelativePath($originalImagePath), File::get($imageFile));

    $image = Image::make($originalImageAbsolutePath);
    if ($image->width() !== $image->height()) {
        Media::cropToSquare($image)->save($imageAbsolutePath);
    } else {
        File::copy($originalImageAbsolutePath, $imageAbsolutePath);
    }

    $user->image_path = $imagePath;
    $user->save();
    return $user;
});


// ---------------- VIDEOS -------------------------------

Route::post('videos', function (Request $request) {
    $videoFile = $request->file('video');
    $fromId    = Cast::toMaybeInt($request->from_id);
    $toId      = Cast::toMaybeInt($request->to_id);

    $video = Video::create([
        'from_id' => $fromId,
        'to_id'   => $toId,
    ]);

    $filePath = "video/$video->id.{$videoFile->getClientOriginalExtension()}";
    Storage::put(Media::getAppStorageRelativePath($filePath), File::get($videoFile));

    $secondsAfterStart = 1;
    $previewPath       = "img/v-$video->id.png";
    $ffVideo           = FFMpeg\FFMpeg::create()->open(Media::getAbsolutePath($filePath));
    $frame             = $ffVideo->frame(FFMpeg\Coordinate\TimeCode::fromSeconds($secondsAfterStart));
    $frame->save(Media::getAbsolutePath($previewPath));

    $video->file_path    = $filePath;
    $video->preview_path = $previewPath;
    $video->save();

    return $video->url;
});

Route::get('videos', function (Request $request) {
    $userId = Cast::toMaybeInt($request->user_id);
    $peerId = Cast::toMaybeInt($request->peer_id);

    $fromId = Cast::toMaybeInt($request->from_id);
    $toId   = Cast::toMaybeInt($request->to_id);

    return Video::with(['fromUser:id,name,image_path', 'toUser:id,name,image_path'])
        ->when($userId, function (Builder $query) use ($userId) {
            return $query->where(Query::orColumns('from_id', $userId, 'to_id', $userId));
        })
        ->when($userId && $peerId, function (Builder $query) use ($userId, $peerId) {
            $query->orWhereWithParenthesis(
                Query::andColumns('from_id', $userId, 'to_id', $peerId),
                Query::andColumns('from_id', $peerId, 'to_id', $userId)
            );
        })
        ->when($fromId, function (Builder $query) use ($fromId) {
            return $query->where('from_id', $fromId);
        })
        ->when($toId, function (Builder $query) use ($toId) {
            return $query->where('to_id', $toId);
        })
        ->latest()
        ->get();
});

// ---------------- DIALOGS -------------------------------

Route::get('dialogs', function (Request $request) {
    $userId = Cast::toMaybeInt($request->user_id);
    if (!$userId) {
        return [];
    }

    return Dialog::whereUserId($userId)
        ->with(['peerUser:id,name,image_path', 'lastVideo'])
        ->get();
});
