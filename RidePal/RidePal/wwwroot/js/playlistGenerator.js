var children = document.getElementsByName("userInp");
var divBoxes

var checkCount = 0;
var checkboxes = document.querySelectorAll("input[type=checkbox][name=userInp]");
let list = [];
var percentage = 0.0;
var restPercentage = 100;
var submitBtn = document.getElementById("submitTrip");
submitBtn.disabled = true;
var gorenInout = document.getElementsByClassName("gorenInput");//проценти в кутийските
//document.getElementById('reset').addEventListener('click', function () {
//    checkCount = 0;
//    percentage = 0.0;
//    restPercentage = 100;
//    for (var i = 0; i < children.length; i++) {


//        children[i].disabled = false;
//        list = []


//    };
//    for (var i = 0; i < children.length; i++) {
//        if (children[i].checked == true) {

//            let index = children[i].id

//            // Get the output text
//            var genre = document.getElementById("getGenre" + index);

//            // If the checkbox is checked, display the output text

//            genre.style.display = "none";
//        };

//    };
//    for (var i = 0; i < dolniInputi.length; i++) {
//        dolniInputi[i].disabled = false;
//    };


//})

checkboxes.forEach(function (box) {
    box.addEventListener("change", function () {
        if (box.checked == true) {
            checkCount++;
        }
        else if (box.checked == false) {
            checkCount--;
        }
        console.log(checkCount);

        if (checkCount == 5) {
            for (var i = 0; i < children.length; i++) {
                if (children[i].checked == false) {

                    children[i].disabled = true;

                }
            }
        }
        else if (checkCount < 5) {
            for (var i = 0; i < children.length; i++) {
                if (children[i].checked == false) {

                    children[i].disabled = false;

                }
            }
        };
    })

});

document.getElementById("openBox").addEventListener('click', function openBoxes() {


    if (checkCount > 0 ) {
       document.getElementById("openBox").disabled = true;
       
        submitBtn.disabled = false;

        for (var i = 0; i < children.length; i++) {
            if (children[i].checked == true) {

                let index = children[i].id

                var checkBox = document.getElementById(index)

                // Get the output text
                var genre = document.getElementById("getGenre" + index);

                // If the checkbox is checked, display the output text
                if (checkBox.checked == true) {
                    genre.style.display = "inline-block";
                    genre.style.margin = "2%";
                    genre.style.marginBottom = "2%";


                }

                checkBox.disabled = true;

                restPercentage = 100;
                percentage = 0.0;

                if (list.length < 6) {

                    list.push(document.getElementById("inp" + index))
                }
                else {
                    alert("You can add up to 5 genres in your playlist")
                }

            }
            else {
                children[i].disabled = true;
            };

        };
        list.forEach((x, i) => x.value = (restPercentage / list.length).toFixed(0))

        console.log(list);


        list.forEach(function (percBox) {
            percBox.addEventListener('change', function calculations(percBox) {

                var elem = percBox.path[0];
                const calc = restPercentage - list.length - 1;
                const error = 'The value for the input is exceeded. You can not pass more than'.concat(calc).concat('%');

                if (elem.value > restPercentage - list.length - 1) {
                    alert(error);
                    elem.value = percentage;
                }
                else {
                    if (list.includes(elem)) {
                        if (list.length > 1 && elem != null) {

                            console.log(percBox.id + " was changed");
                            percentage = (restPercentage - elem.value) / (list.length - 1).toFixed(0);
                            const indexBox = list.indexOf(elem);
                            list.splice(indexBox, 1);
                            restPercentage = restPercentage - elem.value;
                            elem.disabled = true;
                            print.call();
                        }
                        else {
                            var elem = percBox.path[0];
                            const indexBox = list.indexOf(elem);
                            list.splice(indexBox, 1);
                            elem.disabled = true;
                        }
                    }
                }


            });

        });

        function print() {
            list.forEach((x, i) => x.value = percentage.toFixed(0))
            if (list.length == 1) {
                list.forEach((x, i) => x.disabled = true)
            };

        };
    }


});




