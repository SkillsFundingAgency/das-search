/// <binding />
'use strict';

module.exports = function (grunt) {

    // Time how long tasks take. Can help when optimizing build times
    require('time-grunt')(grunt);
    require('load-grunt-tasks')(grunt);

    grunt.initConfig({

        sass: {
            dist: {
                options: {
                    outputStyle: 'compressed',
                    noCache: true,
                    sourceMap: false,
                    precision: 3
                },
                files: [{
                    expand: true,
                    cwd: 'Content/src/styles/',
                    src: '*.scss',
                    dest: 'Content/dist/css/',
                    ext: '.min.css'
                }]
            }
        },

        autoprefixer: {
            options: {
                browsers: ['last 2 versions', 'ie 9'],
                cascade: false,
                map: false
            },
            dist: {
                files: [{
                    expand: true,
                    cwd: 'Content/dist/css/',
                    src: '{,*/}*.css',
                    dest: 'Content/dist/css/'
                }]
            }
        },

        svgmin: {
            dist: {
                files: [{
                    expand: true,
                    cwd: 'Content/src/images/',
                    src: '{,*/}*.svg',
                    dest: 'Content/dist/images/'
                }]
            }
        },

        // Watches files for changes and runs tasks based on the changed files
        watch: {
            styles: {
                    files: ['Content/src/styles/{,*/}*.scss'],
                    tasks: ['sass', 'autoprefixer']
            },

            svg: {
                    files: ['Content/src/images/{,*/}*.svg'],
                    tasks: ['svgmin']
            }
        }
    });

    grunt.loadNpmTasks('grunt-autoprefixer');

    grunt.registerTask('build', [
      'sass',
      'autoprefixer',
      'svgmin'
    ]);

    grunt.registerTask('default', [
      'build'
    ]);
};