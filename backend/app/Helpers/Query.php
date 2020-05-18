<?php

namespace App\Helpers;


use Illuminate\Database\Eloquent\Builder;

class Query
{
    /**
     * @param string $column1
     * @param $value1
     * @param string $column2
     * @param $value2
     * @return \Closure
     */
    public static function orColumns(string $column1, $value1, string $column2, $value2)
    {
        return function (Builder $query) use ($column1, $value1, $column2, $value2) {
            $query->where($column1, $value1)
                ->orWhere($column2, $value2);
        };
    }

    /**
     * @param string $column1
     * @param $value1
     * @param string $column2
     * @param $value2
     * @return \Closure
     */
    public static function andColumns(string $column1, $value1, string $column2, $value2)
    {
        return function (Builder $query) use ($column1, $value1, $column2, $value2) {
            $query->where($column1, $value1)
                ->where($column2, $value2);
        };
    }

}