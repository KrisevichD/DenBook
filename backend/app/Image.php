<?php

namespace App;

use App\Helpers\Media;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Notifications\Notifiable;


/**
 * App\Image
 *
 * @property int $id
 * @property string $name
 * @property string $file_path
 * @property int $width
 * @property int $height
 * @property string $extension
 * @property \Illuminate\Support\Carbon|null $created_at
 * @property \Illuminate\Support\Carbon|null $updated_at
 * @property-read mixed $url
 * @property-read \Illuminate\Notifications\DatabaseNotificationCollection|\Illuminate\Notifications\DatabaseNotification[] $notifications
 * @property-read int|null $notifications_count
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image newModelQuery()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image newQuery()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image query()
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image whereCreatedAt($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image whereExtension($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image whereFilePath($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image whereHeight($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image whereId($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image whereName($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image whereUpdatedAt($value)
 * @method static \Illuminate\Database\Eloquent\Builder|\App\Image whereWidth($value)
 * @mixin \Eloquent
 * @property-read \Illuminate\Database\Eloquent\Collection|\App\User[] $users
 * @property-read int|null $users_count
 * @property-read \Illuminate\Database\Eloquent\Collection|\App\Video[] $videos
 * @property-read int|null $videos_count
 */
class Image extends Model
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
        'file_path', 'updated_at'
    ];

    protected $attributes = [
        'file_path' => 'error',
    ];

    public $appends = [
        'url'
    ];

    public function videos()
    {
        return $this->morphedByMany(Video::class, 'imageable');
    }

    public function users()
    {
        return $this->morphedByMany(User::class, 'imageable');
    }

    public function getUrlAttribute()
    {
        return Media::getPublicUrl($this->file_path);
    }

}

