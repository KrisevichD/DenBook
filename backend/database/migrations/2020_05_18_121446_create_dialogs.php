<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;

class CreateDialogs extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('dialogs', function (Blueprint $table) {
            $table->id();
            $table->unsignedInteger('twin_dialog_id')->nullable();
            $table->unsignedInteger('user_id');
            $table->unsignedInteger('peer_id');
            $table->unsignedInteger('last_video_id');
            $table->timestamps();

            \App\User::fkOnId($table, 'user_id');
            \App\User::fkOnId($table, 'peer_id');

            $table->foreign('last_video_id')
                ->on('videos')
                ->references('id')
                ->onUpdate('cascade');

            $table->unique(['user_id', 'peer_id']);
            $table->index('user_id', 'dialogs_user_id_idx', 'hash');
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('dialogs');
    }
}
