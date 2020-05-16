<?php

use App\User;
use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;

class CreateVideosTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('videos', function (Blueprint $table) {
            $table->id();
            $table->unsignedInteger('from_id');
            $table->unsignedInteger('to_id');
            $table->string('file_path');
            $table->text('description')->nullable();
            $table->timestamps();


            User::fkOnId($table, 'from_id');
            User::fkOnId($table, 'to_id');
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('videos');
    }
}
