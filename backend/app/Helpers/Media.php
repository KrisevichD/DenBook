<?php

namespace App\Helpers;

use Intervention\Image\Image;


abstract class Media
{
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

    public static function cropToSquare(Image $image)
    {
        $width  = $image->width();
        $height = $image->height();
        if ($width == $height) {
            return $image;
        }

        $size  = min($width, $height);
        $delta = intdiv($width + $height, 2) - $size;
        $x     = $y = 0;
        $width < $height ? ($y = $delta) : ($x = $delta);

        $image->crop($size, $size, $x, $y);
        return $image;
    }
}