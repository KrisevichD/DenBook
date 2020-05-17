<?php

namespace App\Helpers;


class Cast
{
    public static function toMaybeInt($value): ?int
    {
        return $value ? (int)$value : null;
    }
}