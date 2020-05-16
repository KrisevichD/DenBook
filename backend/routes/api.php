<?php

use App\User;
use App\Video;
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

Route::middleware('auth:api')->get('/user', function (Request $request) {
    return $request->user();
});


Route::post('register', function (Request $request) {
    $name     = $request->name;
    $password = $request->password;

    // TODO: convert to transaction
    if (User::whereName($name)->exists()) {
        return 1;
    }
    User::create([
        'name'     => $name,
        'password' => $password
    ]);

    return 0;
});


Route::get('login', function (Request $request) {
    $name     = $request->name;
    $password = $request->password;

    $user = User::whereName($name)->first();
    if (!$user) {
        return 1;
    }

    if ($user->password !== $password) {
        return 2;
    }

    return $user->toJson();
});


Route::get('users', function (Request $request) {
    return User::all();
});


Route::get('videos', function (Request $request) {
    $id = $request->id;

    return Video::findOrFail($id);
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