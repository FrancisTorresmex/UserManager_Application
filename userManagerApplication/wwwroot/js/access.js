var idUser = 0;
var typeInputPass = true;

function login() {

    var loginForm = document.getElementById("userLogin");

    var email = loginForm.elements["Email"].value;
    var password = loginForm.elements["Password"].value;

    if (Validate('.requiredAccess').length === 0) {
        var obj = {
            email: email,
            password: password
        };

        requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(obj)
        };

        fetch('/Access/Login/', requestOptions)
            .then(function (response) {
                if (!response.ok) {
                    alertAnimatedCustom(error, 'error', 'An error occurred');
                }

                return response.json();

            })
            .then(function (response) {
                /*window.location.href = 'Home/';*/

                if (!response.success) {
                    alertAnimatedCustom(response.error, 'error', 'An error occurred');
                } else {
                    window.location.href = response.data;
                }


            })
            .catch(function (error) {
                alertAnimatedCustom(error, 'error', 'An error occurred');
            });
    }

    
}

function seePassword(event) {
    event.stopPropagation(); //prevents the execution of the click event to submit form

    var inputPass = document.getElementById("inpAccessPassword");

    if (typeInputPass)
        inputPass.type = "text";     
    else
        inputPass.type = "password";

    typeInputPass = !typeInputPass;
    
}
