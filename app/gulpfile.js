
let gulp = require("gulp"),
    fs = require("fs"),
    less = require("gulp-less"),
    cleanCSS = require('gulp-clean-css');


gulp.task("lessFile", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/filechoose/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/filechoose/css/'));
});

gulp.task("lessConfig", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/config/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/config/css/'));
});

gulp.task("lessUsers", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/users/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/users/css/'));
});

gulp.task("lessIdentity", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/identity/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/identity/css/'));
});

gulp.task("lessDesktop", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/desktop/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/desktop/css/'));
});

gulp.task("lessMain", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/main/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/main/css/'));
});

gulp.task("lessIndex", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/index/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/index/css/'));
});

gulp.task("lessStore", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/store/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/store/css/'));
});

gulp.task("lessUiControls", function () {
    return gulp.src('wwwroot/lib/mtd-ordermaker/ui/less/mtd*.less').pipe(less()).pipe(gulp.dest('wwwroot/lib/mtd-ordermaker/ui/css/'));
});

