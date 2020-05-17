<?php

namespace App;

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
 * @property-read mixed $public_url
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
        'file_path'
    ];

    protected $attributes = [
        'file_path' => 'error'
    ];

    public $appends = [
        'url'
    ];

    public function getPublicUrlAttribute()
    {
        return self::getPublicUrl($this->file_path);
    }

    public static function getPublicUrl(string $filePath): string
    {
        return config('app.url') . "/media/$filePath";
    }

    public static function getAbsolutePath(string $filePath): string
    {
        return storage_path("app/media/$filePath");
    }

    public static function getAppStorageRelativePath(string $filePath): string
    {
        return "media/$filePath";
    }
}

