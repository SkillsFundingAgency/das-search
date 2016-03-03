'use strict';

module.exports = function (grunt) {

    // Time how long tasks take. Can help when optimizing build times
    require('time-grunt')(grunt);

    // Automatically load required Grunt tasks
    require('jit-grunt')(grunt, {
        useminPrepare: 'grunt-usemin'
    });

    // Configurable paths
    var config = {
        src: 'src',
        dist: 'dist'
    };

    grunt.initConfig({

        // Project settings
        config: config,

        // Watches files for changes and runs tasks based on the changed files
        watch: {
            gruntfile: {
                files: ['Gruntfile.js']
            },
            styles: {
                files: ['<%= config.app %>/styles/{,*/}*.css'],
                tasks: []
            }
        },

        // Empties folders to start fresh
        clean: {
            images: {
                files: [{
                    src: [
                      '<%= config.dist %>/images/*'
                    ]
                }]
            }
        },

        imagemin: {
            dist: {
                files: [{
                    expand: true,
                    cwd: '<%= config.src %>/images',
                    src: '{,*/}*.{gif,jpeg,jpg,png}',
                    dest: '<%= config.dist %>/images'
                }]
            }
        }//,

        //svgmin: {
        //    dist: {
        //        files: [{
        //            expand: true,
        //            cwd: '<%= config.src %>/images',
        //            src: '{,*/}*.svg',
        //            dest: '<%= config.dist %>/images'
        //        }]
        //    }
        //}
    });

    grunt.loadNpmTasks('grunt-clean');
    grunt.loadNpmTasks('grunt-contrib-imagemin');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.registerTask('build', [
      'clean',
      'imagemin',
      'watch'
    ]);

    grunt.registerTask('default', [
      'build'
    ]);
};