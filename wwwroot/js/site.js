// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#CalendarCompact").datepicker();
  

window.onscroll = (function () {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.getElementById("gotop").style.display = "block";
    }
    else { document.getElementById("gotop").style.display = "none"; }
});


$('#site-header .btn-expand').on('click', btnExpandClick);
$('#site-header .btn-search-mobile').on('click', btnSearchClick);
$('#site-header .site-header__tools .fa-times').on('click', btnCloseClick);
$('#site-header .nav-search > .nav-link').on('click', btnToggleSearchDesktop);

// site mask click
$('.site-mask').on('click', siteMaskClick);



function btnExpandClick(e) {
    e.preventDefault();
    e.stopPropagation();
    expandNav();
}

function expandNav() {
    $('#site-header .nav').toggle();
    $('.btn-expand .fa-bars').toggle();
    $('.btn-expand .fa-times').toggle();
    // hide search
    $('.btn-search-mobile .fa-search').show();
    $('.btn-search-mobile .fa-times').hide();
    $('#site-header .input-wrap-mobile').hide();
    $('body').addClass('is-expanded')
}

function btnSearchClick(e) {
    e.preventDefault();
    e.stopPropagation();
    expandSearch();
    $('#searchInput-mobile').focus();
}

function btnCloseClick(e) {
    e.preventDefault();
    e.stopPropagation();
    siteMaskClick();
}

function btnToggleSearchDesktop(e) {
    e.preventDefault();
    e.stopPropagation();
    toggleSearchDesktop();
}

function expandSearch() {
    $('#site-header .input-wrap-mobile').toggle();
    $('.btn-search-mobile .fa-search').toggle();
    $('.btn-search-mobile .fa-times').toggle();
    // hide nav
    $('.btn-expand .fa-bars').show();
    $('.btn-expand .fa-times').hide();
    $('#site-header .nav').hide();
    $('body').addClass('is-expanded')
}

function siteMaskClick() {
    // e.preventDefault();
    // e.stopPropagation();
    $('#site-header .nav').hide();
    $('.btn-expand .fa-bars').show();
    $('.btn-expand .fa-times').hide();
    $('.btn-search-mobile .fa-search').show();
    $('.btn-search-mobile .fa-times').hide();
    $('#site-header .input-wrap-mobile').hide();
    $('body').removeClass('is-expanded');
}

function toggleSearchDesktop() {
    $('.nav-search > .nav-link').toggleClass('is-active');
    $('.nav-search .search-wrap').toggleClass('is-active');
}

//front image url
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image_upload_preview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#customFile").change(function () {
    readURL(this);
});

$("#customFile").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

var highlightSlider = $('.zone--highlight .slider')
if (highlightSlider.length > 0) {
    highlightSlider.bxSlider({
        auto: 1,
        minSlides: 1,
        maxSlides: 1,
        pager: 0,
        nextText: '',
        prevText: '',
        nextSelector: ".slider-next",
        prevSelector: ".slider-prev",
        touchEnabled: false,
    });
}


