<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Notifications\Notifiable;


/**
 * App\Dialog
 *
 * @property int $id
 * @property int $twin_dialog_id
 * @property int $user_id
 * @property int $peer_id
 * @property int $last_video_id
 * @property \Illuminate\Support\Carbon|null $created_at
 * @property \Illuminate\Support\Carbon|null $updated_at
 * @property-read \Illuminate\Notifications\DatabaseNotificationCollection|\Illuminate\Notifications\DatabaseNotification[] $notifications
 * @property-read int|null $notifications_count
 * @property-read \App\User $peerUser
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog newModelQuery()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog newQuery()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog query()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog whereCreatedAt($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog whereId($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog whereLastVideoId($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog wherePeerId($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog whereTwinDialogId($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog whereUpdatedAt($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Dialog whereUserId($value)
 * @mixin \Eloquent
 */
class Dialog extends Model
{
    use Notifiable;

    /**
     * The attributes that are not mass assignable.
     *
     * @var array
     */
    protected $guarded = ['id'];

    /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */
    protected $hidden = [
        'id', 'twin_dialog_id', 'created_at', 'updated_at'
    ];


    public function peerUser()
    {
        return $this->hasOne(User::class, 'id', 'peer_id');
    }

    public function lastVideo()
    {
        return $this->hasOne(Video::class, 'id', 'last_video_id');
    }


    // TODO: same for create
    public static function updateOrCreate($attributes, $values = [])
    {
        /** @var Dialog $model */
        $model = static::query()->updateOrCreate($attributes, $values);
        if (!$model->twin_dialog_id && $model->user_id && $model->peer_id) {
            $twinModel = self::create(array_merge($values, [
                'user_id'        => $model->peer_id,
                'peer_id'        => $model->user_id,
                'twin_dialog_id' => $model->id
            ]));

            $model->twin_dialog_id = $twinModel->id;
            $model->save();
            return $model;
        }

        static::query()->updateOrCreate(array_merge($attributes, [
            'user_id' => $model->peer_id,
            'peer_id' => $model->user_id,
        ]), $values);
        return $model;
    }
}

