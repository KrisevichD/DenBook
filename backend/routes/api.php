<?php

use App\User;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;

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


// TODO: post -> get
Route::post('login', function (Request $request) {
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


// TODO: post -> get
Route::post('users', function (Request $request) {
    return User::all();
});
