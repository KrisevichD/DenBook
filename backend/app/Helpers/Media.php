<?php

namespace App\Helpers;


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
}