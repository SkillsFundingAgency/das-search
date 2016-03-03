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
                    cwd: 'src/styles/',
                    src: '*.scss',
                    dest: 'css/',
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
                    cwd: 'css/',
                    src: '{,*/}*.css',
                    dest: 'css/'
                }]
            }
        },

        // Watches files for changes and runs tasks based on the changed files
        watch: {
            styles: {
                files: ['src/styles/{,*/}*.scss'],
                tasks: ['sass', 'autoprefixer']
            }
        }
    });

    grunt.registerTask('build', [
      'sass',
      'autoprefixer'
    ]);

    grunt.registerTask('default', [
      'build'
    ]);
};