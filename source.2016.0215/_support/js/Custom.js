$(document).ready(function () {
    $('a[href^="http"]').each(function () {
        if (!$(this).attr('href').match(/(lib(rary)?|www|tilt|search).colostate.edu/))
            $(this).attr('title', $(this).attr('title') + " external link");
    });
    $('a[target="_blank"], a[target="_new"]').each(function () {
        $(this).attr('title', $(this).attr('title') + " opens in a new window");
    });
});