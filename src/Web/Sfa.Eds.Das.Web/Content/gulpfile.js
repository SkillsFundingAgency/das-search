var gulp          = require('gulp'),
    chalk         = require('chalk'),
    sass          = require('gulp-ruby-sass'),
    autoprefixer  = require('gulp-autoprefixer'),
    plumber       = require('gulp-plumber'),
    runSequence   = require('run-sequence'),
    browserSync   = require('browser-sync').create(),
    rename        = require('gulp-rename'),
    minifyCss     = require('gulp-minify-css'),
    gutil         = require('gulp-util');



gulp.task('default', function() {
  console.log('');
  console.log(chalk.blue('DAS gulp'));
  console.log(chalk.blue('usage :'));
  console.log(chalk.blue('  $> gulp dev   -> to start dev server'));
  console.log('');
});


gulp.task('styles', function() {
  return sass([
    './src/styles/screen.scss'
  ], { style: 'expanded' })
    .pipe(plumber())
    .pipe(autoprefixer({
      browsers: ['last 2 versions'],
      cascade: false
    }))
    .pipe(minifyCss({compatibility: 'ie8'}))
    .pipe(rename({
      suffix: '.min'
    }))
    .pipe(gulp.dest('./css/'))
});

gulp.task('fonts', function() {
  return sass([
    './src/styles/fonts.scss'
  ], { style: 'expanded' })
    .pipe(plumber())
    .pipe(autoprefixer({
      browsers: ['last 2 versions'],
      cascade: false
    }))
    .pipe(gulp.dest('./css/'))
});


/* Public methods */

gulp.task('dev', function() {
  runSequence(
    ['watch'],
    ['styles'],
    ['fonts']);
});


gulp.task('watch', function() {
    gulp.watch(['./src/styles/**/*.scss'], ['styles', 'fonts']);
});
