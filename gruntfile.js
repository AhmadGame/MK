/* global module */

module.exports = function(grunt) {

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        jshint: {
            appFiles: [
                '/js/*.js'
            ],
            options: {
                reporter: require('jshint-stylish'),

                // ENFORCING
                "bitwise": true,
                "camelcase": true,
                "curly": true,
                "eqeqeq": true,
                "forin": true,
                "freeze": true,
                "immed": true,
                "latedef": true,
                "newcap": true,
                "noarg": true,
                "noempty": true,
                "nonbsp": true,
                "nonew": true,
                "plusplus": true,
                "quotmark": false,
                "trailing": true,
                "undef": true,
                "unused": "vars",
                // "strict": true,

                // GLOBALS
                "predef": [
                    "_",
                    "clearTimeout",
                    "document",
                    "jQuery",
                    "ko",
                    "Modernizr",
                    "moment",
                    "setTimeout",
                    "window"
                ]
            },
            gruntfile: {
                files: {
                    src: ['Gruntfile.js']
                },
                options: {
                    "predef": ["require"]
                }
            }
        },

        clean: {
            hooks: ['.git/hooks/pre-commit']
        },

        shell: {
            hooks: {
                command: 'cp git-hooks/pre-commit.sh .git/hooks/pre-commit'
            }
        }
    });

    // Load plugins.
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-shell');

    // Tasks
    grunt.registerTask('installgithooks', ['clean:hooks', 'shell:hooks']);
    //grunt.registerTask('watcher', ['less:app', 'less:admin', 'copy:app', 'watch']);

    // Default task(s).
    //grunt.registerTask('default', ['watcher']);

};