<?php

namespace App\Providers;

use Illuminate\Database\Eloquent\Builder;
use Illuminate\Support\ServiceProvider;

class AppServiceProvider extends ServiceProvider
{
    /**
     * Register any application services.
     *
     * @return void
     */
    public function register()
    {
        //
    }

    /**
     * Bootstrap any application services.
     *
     * @return void
     */
    public function boot()
    {

        /**
         * Surrounds conditions with parenthesis
         *
         * @return Builder
         * @instantiated
         */
        Builder::macro('orWhereWithParenthesis',

            /**
             * @param \Closure $clause1
             * @param \Closure $clause2
             * @return Builder
             */
            function ($clause1, $clause2) {
                /* @var $this Builder */
                return $this->where(function (Builder $query) use ($clause1, $clause2) {
                    return $query->where($clause1)
                        ->orWhere($clause2);
                });
            });
    }
}
