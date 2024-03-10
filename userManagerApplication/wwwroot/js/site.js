/*
    variable
*/
let objectUserData = null;
let tblUsers = null;
let modalUpdateUserInitialize = null;
let idUserTemp = 0;

let userId = 0;
let modalUserPagesInitialize = null;


/* 
    Initial js: Here will be the functions required when starting the application 
*/


$(document).ready(function () {

    var culture = getCookie("Language");
    if (culture != '') {
        getUserPages();

        var imgLanguage = document.getElementById("imgLanguageMenu");
        if (imgLanguage) {
            var cultureUrlImg = getCookie("urlLanguage");
            imgLanguage.src = cultureUrlImg;
        }
        
    }

    //toggle menu mobile
    const mobileMenuButton = document.getElementById('mobile-menu');
    const navbarMobile = document.querySelector('.navbarCustom');
    const toggleMenubtn = document.getElementById("btnMobileToggleMenu").querySelector("i");

    mobileMenuButton.addEventListener('click', function () {
        
        /*mobileMenuButton.style.display = (mobileMenuButton.style.display === 'none' || mobileMenuButton.style.display === '') ? 'block' : 'none';*/
        navbarMobile.style.display = (navbarMobile.style.display === 'none' || navbarMobile.style.display === '') ? 'block' : 'none';

        if (toggleMenubtn.classList.contains("fa-bars")) {
            toggleMenubtn.classList.remove("fa-bars");
            toggleMenubtn.classList.add("fa-times");
        } else {
            toggleMenubtn.classList.remove("fa-times");
            toggleMenubtn.classList.add("fa-bars");
        }

    });
    //end toggle menu mobile

    //allows using partial indexes of controllers
    $("nav.navbarCustom ul.sidenavUl").on('click', 'a', function (e) {
        e.preventDefault();

        //close toggle menu when pressing, only if mobile
        //if (mobileMenuButton.style.display === 'block') {
        //    navbarMobile.style.display = (navbarMobile.style.display === 'block' || navbarMobile.style.display === '') ? 'block' : 'none';
        //}
        

        // Add the background to the parent <li> element of the clicked <a> link with jquery
        $("nav.navbarCustom ul.sidenavUl li").removeClass('selectedUlMenu');
        $(this).closest('li').addClass('selectedUlMenu');

        var url = $(this).attr("href");
        var partialView = url + "/PartialIndex/"

        // Utilizar AJAX para obtener el contenido
        $.ajax({
            url: partialView,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                // Actualizar el contenedor específico con el contenido obtenido
                $('#dynamicContentContainer').html(result.data);
            },
            error: function () {
                alertAnimatedCustom('Se produjo un error al obtener el contenido', 'error', 'Error');
            }
        });
    });

    


    //Lenguage sub menu
    var imgLanguageMenu = document.getElementById("imgLanguageMenu");
    var submenu = document.getElementById("menuLanguageOption");

    // Agregar evento de clic a la imagen
    if (imgLanguageMenu != null) {
        imgLanguageMenu.addEventListener("click", function () {
            // Alternar la visibilidad del submenú
            if (submenu.classList.contains("hiddenMenuLanguage")) {
                submenu.classList.remove("hiddenMenuLanguage");
            } else {
                submenu.classList.add("hiddenMenuLanguage");
            }
        });

        // Cerrar el submenú si se hace clic fuera de él
        document.addEventListener("click", function (event) {
            if (!submenu.contains(event.target) && event.target !== imgLanguageMenu) {
                submenu.classList.add("hiddenMenuLanguage");
            }
        });
    }
    
    //end Lenguage sub menu

});


//reusable fetch call
function functionFetch(url, data, methodType, funcSuccess) { //funcSuccess = return function, data = {object}

    var requestOptions = {
        method: methodType,
        headers: {
            'Content-Type': 'application/json',
            //'Authorization': 'Bearer ' + token
        },
         //body: JSON.stringify(data)
        };

    if (methodType !== 'GET') {
        requestOptions.body = JSON.stringify(data);
    }

    fetch(url, requestOptions)
        .then(function (response) {
            if (!response.ok) {
                /*throw new Error(response.statusText);*/
                alertAnimatedCustom(error, 'error', 'An error occurred');
            }

            return response.json();

        })
        .then(function (response) {
            funcSuccess(response);
        })
        .catch(function (error) {
            
            alertAnimatedCustom(error, 'error', 'An error occurred');
        });
}

//reusable: update datatable row
//isObject = true if you send an entire object, or false if you only want to update a part of that object
function updateRowDataTable(tbl, valueSearch, valueUpdated, updateAllObjectBool) {

    var row = tbl.row('tr[data-id="' + valueSearch + '"]');

    if (row) {

        if (updateAllObjectBool) {
            tbl.row('tr[data-id="' + valueSearch + '"]').data(valueUpdated).draw();
        }
        else {
            var rowData = row.data();
            for(var key in valueUpdated) {
                if (rowData.hasOwnProperty(key)) {
                    rowData[key] = valueUpdated[key];
                }
            }
            row.data(rowData).draw();
        }

        
        
    }
    else {
        alertAnimatedCustom('Row not found', 'error', 'An error occurred');
    }
}

//reusable: update all datatable
function updateDataTable(tbl, data) {
    tbl.api().clear().rows.add(data).draw();
}

//Alerts sweet alerts: Basic
function alertBasicCustom(message, icon, title) {
    swal.fire({
        icon: icon,
        title: title,
        text: message
    });
}

//Alerts sweet alerts: animation and basic
function alertAnimatedCustom(message, icon = 'success', title = 'Alert') {
    Swal.fire({
        title: title,
        text: message,
        icon: icon,
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        }
    })
}

//Values required
function Validate(classElement) {

    var required = document.querySelectorAll(classElement);
    var errorLst = [];

    for (var i = 0; i < required.length; i++) {

        var elementError = document.getElementById(required[i].id);

        if (required[i].value.trim() === '') {

            elementError.style.borderColor = 'red';
            errorLst.push(required[i].id);
        }
        else
            elementError.style.borderColor = '';
    }

    return errorLst;
}

function resetColorField(classElement) {
    var required = document.querySelectorAll(classElement);

    for (var i = 0; i < required.length; i++) {

        var element = document.getElementById(required[i].id);
        element.style.borderColor = '';
    }
}

//end values required


//Cookies js

// Función para establecer o actualizar una cookie
function setCookie(nombre, valor, diasParaExpirar) {
    var fechaExpiracion = new Date();
    fechaExpiracion.setTime(fechaExpiracion.getTime() + (diasParaExpirar * 24 * 60 * 60 * 1000));
    var expira = "expires=" + fechaExpiracion.toUTCString();
    document.cookie = nombre + "=" + valor + ";" + expira + ";path=/";
}

// Función para obtener el valor de una cookie
function getCookie(nombre) {
    var nombreCookie = nombre + "=";
    var cookies = decodeURIComponent(document.cookie).split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].trim();
        if (cookie.indexOf(nombreCookie) === 0) {
            return cookie.substring(nombreCookie.length, cookie.length);
        }
    }
    return "";
}

// Función para borrar una cookie
function deleteCookie(nombre) {
    document.cookie = nombre + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}

//end Cookies js


//Get user pages




function getUserPages() {

    functionFetch("/Pages/GetAllUserPages/", {}, "GET", successGetUserPages);

}

function successGetUserPages(data) {
    if (data.success) {

        var lstScreen = data.data;
        var menu = document.querySelector(".sidenavUl");
        lstScreen.forEach(function (item) {

            var li = document.createElement("li");
            var a = document.createElement("a");

            a.setAttribute("href", item.url);
            a.textContent = item.title;
            li.appendChild(a);
            menu.appendChild(li);
        });
    }
    else
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
}


function changeLanguageMenu(culture, urlImg) {

    //var language = getCookie("Language");
    setCookie("Language", culture, Date.now());
    setCookie("urlLanguage", urlImg, Date.now());

    var submenu = document.getElementById("menuLanguageOption");
    submenu.classList.add("hiddenMenuLanguage");

    var imgLanguage = document.getElementById("imgLanguageMenu");
    imgLanguage.src = urlImg;

    window.location.href = `/?culture=${culture}`
}

function closeSesion() {
    deleteCookie("Language");
    deleteCookie("Token");
    deleteCookie("IdUser");

    window.location.href = `/?culture=${culture}`
}


