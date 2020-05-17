<?php

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


Route::middleware('auth:api')->get('/user', function (Request $request) {
    return $request->user();
});


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


Route::get('users', function (Request $request) {
    return User::all();
});


Route::post('videos', function (Request $request) {
    $video  = $request->file('video');
    $fromId = $request->from_id;
    $toId   = $request->to_id;

    $fileContent = File::get($video);
    $fileName    = 'den4ik-' . now()->getTimestamp() . '.' . $video->getClientOriginalExtension();
    Storage::put('media/' . $fileName, $fileContent);

    $video = Video::create([
        'from_id'   => $fromId,
        'to_id'     => $toId,
        'file_path' => $fileName
    ]);
    return $video->url;
});


Route::get('videos', function (Request $request) {
    $fromId = $request->from_id ? (int)$request->from_id : null;
    $toId   = $request->to_id ? (int)$request->to_id : null;
    $userId = $request->user_id ? (int)$request->user_id : null;

    if (!$fromId && !$toId && !$userId) {
        return Video::all();
    }

    return Video::when($userId, function (Builder $query) use ($userId) {
        return $query->where('from_id', $userId)
            ->orWhere('to_id', $userId);
    })->when($fromId, function (Builder $query) use ($fromId) {
        info(gettype($fromId));
        return $query->where('from_id', $fromId);
    })->when($toId, function (Builder $query) use ($toId) {
        return $query->where('to_id', $toId);
    })->get();
});