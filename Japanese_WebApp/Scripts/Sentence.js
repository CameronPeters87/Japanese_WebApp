﻿
$(function () {

    //var search = $('input#search').val();
    //var searchLang = $('p#lang').text();

    //if (searchLang == "Eng") {
    //    var html = $('td.eng').html();
    //    $('td.eng').html(html.replace(new RegExp("(" + search + ")", "gi"), '<strong>$&</strong>'));

    //    //$('td.eng').replace(new RegExp("(" + search + ")", "gi"), '<strong>$&</strong>');

    //}
    //else {
    //    var html = $('td.jap').html();
    //    $('td.jap').html(html.replace(/search/gi, '<strong>$&</strong>'));
    //}

     //sentence.replace(new RegExp("(" + query + ")", "gi"), '<bld>$1</bld>');

    var playing;

    $('.action-vol').mouseover(function () {
        if (!playing) {
            $(this).css("color", "rgba(0, 0, 255, 1)");
        }
    });

    $('.action-vol').mouseleave(function () {
        if (!playing) {
            $(this).css("color", "rgba(0, 0, 255, 0.5)");
        }
    });

    $('.heart').mouseover(function () {
        $(this)
            .removeClass("fa-heart-o")
            .addClass("fa-heart")
            .css("color", "rgba(255, 0, 0, 1)");
    });

    $('.heart').mouseleave(function () {
        $(this)
            .removeClass("fa-heart")
            .addClass("fa-heart-o")
            .css("color", "rgba(255, 0, 0, 1)");
    });

    $('.action-vol').click(function () {

        var element = $(this);
        playing = true;

        $(this).css("color", "rgba(0, 0, 255, 1)");

        var audio = {};
        audio["walk"] = new Audio();
        audio["walk"].src = $(this).attr("data-audio");
        audio["walk"].play();

        audio["walk"].addEventListener("ended", function () {
            $(element).css("color", "rgba(0, 0, 255, 0.5)");
            playing = false;
        })

        //if (audio["walk"].onended(function () { }) {
        //    alert("hello");
        //    $(this).css("color", "rgba(0, 0, 255, 0.5)");
        //}
    });
});