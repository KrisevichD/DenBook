<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;

class DialogLastVideoCreatedAt extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('dialogs', function (Blueprint $table) {
            $table->timestamp('last_video_created_at')->nullable();
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        if (Schema::hasColumn('dialogs', 'last_video_created_at')) {
            Schema::table('dialogs', function (Blueprint $table) {
                $table->dropColumn('last_video_created_at');
            });
        }
    }
}
