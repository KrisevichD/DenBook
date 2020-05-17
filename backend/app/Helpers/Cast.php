<?php

namespace App\Helpers;


abstract class Cast
{
    public static function toMaybeInt($value): ?int
    {
        return $value ? (int)$value : null;
    }
}