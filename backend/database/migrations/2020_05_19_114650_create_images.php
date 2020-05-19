<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;

class CreateImages extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('images', function (Blueprint $table) {
            $table->id();
            $table->string('name');
            $table->string('file_path');
            $table->unsignedInteger('width');
            $table->unsignedInteger('height');
            $table->string('extension');
            $table->timestamps();
        });

        Schema::create('imageables', function (Blueprint $table) {
            $table->integer('image_id');
            $table->integer('imageable_id')->unsigned();
            $table->string('imageable_type', 100);
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('images');
        Schema::dropIfExists('imageables');
    }
}
