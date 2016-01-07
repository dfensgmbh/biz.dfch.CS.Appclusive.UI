
// Navbar on multiple lines covers the body.
// See http://stackoverflow.com/questions/19741145/bootstrap-navbar-fixed-top-covers-content
jQuery(window).resize(function () {
    $('#godown').height($("#navMenu").height() - 43); // 43: 5 pixels of space between navbar and body. 50: without 2 pixels of space.
});

$('#godown').height($("#navMenu").height() - 43); // 43: 5 pixels of space between navbar and body. 50: without 2 pixels of space.


