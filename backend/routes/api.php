<?php

use App\Helpers\Cast;
use App\User;
use App\Video;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Http\Request;

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

    return $user->toJson();
});


// ---------------- USERS -------------------------------

Route::get('users', function (Request $request) {
    $userId = Cast::toMaybeInt($request->user_id);

    if ($userId) {
        $user = User::find($userId);
        return $user ? $user->toJson() : USERS_CODE_NO_USER;
    }

    return User::all();
});


// ---------------- VIDEOS -------------------------------

Route::post('videos', function (Request $request) {
    $file   = $request->file('video');
    $fromId = Cast::toMaybeInt($request->from_id);
    $toId   = Cast::toMaybeInt($request->to_id);

    $video = Video::create([
        'from_id' => $fromId,
        'to_id'   => $toId,
    ]);

    $filePath    = "video/$video->id.{$file->getClientOriginalExtension()}";
    $fileContent = File::get($file);
    Storage::put(Video::getAppStorageRelativePath($filePath), $fileContent);

    $secondsAfterStart = 1;
    $previewPath       = "img/v-$video->id.png";
    $ffVideo           = FFMpeg\FFMpeg::create()->open(Video::getAbsolutePath($filePath));
    $frame             = $ffVideo->frame(FFMpeg\Coordinate\TimeCode::fromSeconds($secondsAfterStart));
    $frame->save(Video::getAbsolutePath($previewPath));

    $video->file_path    = $filePath;
    $video->preview_path = $previewPath;
    $video->save();

    return $video->url;
});

Route::get('videos', function (Request $request) {
    $userId = Cast::toMaybeInt($request->user_id);
    $fromId = Cast::toMaybeInt($request->from_id);
    $toId   = Cast::toMaybeInt($request->to_id);

    return Video::with(['fromUser', 'toUser'])
        ->when($userId, function (Builder $query) use ($userId) {
            return $query->where('from_id', $userId)
                ->orWhere('to_id', $userId);
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
