<?php

namespace App;

use App\Helpers\Media;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Notifications\Notifiable;


/**
 * App\Video
 *
 * @property int $id
 * @property int $from_id
 * @property int $to_id
 * @property string $file_path
 * @property string|null $description
 * @property \Illuminate\Support\Carbon|null $created_at
 * @property \Illuminate\Support\Carbon|null $updated_at
 * @property-read mixed $url
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video newModelQuery()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video newQuery()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video query()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video whereCreatedAt($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video whereDescription($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video whereFilePath($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video whereFromId($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video whereId($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video whereToId($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video whereUpdatedAt($value)
 * @mixin \Eloquent
 * @property string|null $preview_path
 * @property-read \Illuminate\Notifications\DatabaseNotificationCollection|\Illuminate\Notifications\DatabaseNotification[] $notifications
 * @property-read int|null $notifications_count
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Video wherePreviewPath($value)
 * @property-read mixed $preview_url
 * @property-read \App\User $fromUser
 * @property-read \App\User $toUser
 */
class Video extends Model
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
        'file_path', 'preview_path', 'updated_at'
    ];

    protected $attributes = [
        'file_path'    => 'error',
        'preview_path' => 'error',
    ];

    public $appends = [
        'url', 'preview_url'
    ];

    public function fromUser()
    {
        return $this->belongsTo(User::class, 'from_id', 'id');
    }

    public function toUser()
    {
        return $this->belongsTo(User::class, 'to_id', 'id');
    }

    public function getUrlAttribute()
    {
        return Media::getPublicUrl($this->file_path);
    }

    public function getPreviewUrlAttribute()
    {
        return Media::getPublicUrl($this->preview_path);
    }

    public static function create($attributes = [])
    {
        $model = static::query()->create($attributes);
        ['from_id' => $fromId, 'to_id' => $toId] = $attributes;
        if (!$fromId || !$toId) {
            return $model;
        }

        Dialog::updateOrCreate([
            'user_id' => $fromId,
            'peer_id' => $toId,
        ], [
            'last_video_id' => $model->id
        ]);

        return $model;
    }


}

