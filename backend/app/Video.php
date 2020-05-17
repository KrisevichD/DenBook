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
        'file_path',
        'preview_path',
        'updated_at'
    ];

    protected $attributes = [
        'file_path'    => 'error',
        'preview_path' => 'error',
    ];

    public $appends = [
        'url', 'preview_url'
    ];

    public function getUrlAttribute()
    {
        return self::getPublicUrl($this->file_path);
    }

    public function getPreviewUrlAttribute()
    {
        return self::getPublicUrl($this->preview_path);
    }

    public static function getPublicUrl(?string $filePath): ?string
    {
        return $filePath ? config('app.url') . "/media/$filePath" : null;
    }

    public static function getAbsolutePath(string $filePath): string
    {
        return storage_path("app/media/$filePath");
    }

    public static function getAppStorageRelativePath($filePath): string
    {
        return "media/$filePath";
    }
}

