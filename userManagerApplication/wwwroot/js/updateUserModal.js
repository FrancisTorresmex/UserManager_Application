

if ($('#btnUserPages').length) {
    $(document).ready(function () {

        /* edit user pages */
        
        //let modalUpdateUser = document.getElementById('updateUserModal');
        //modalUpdateUserInitialize = new bootstrap.Modal(modalUpdateUser);

        document.getElementById("btnUserPages").addEventListener("click", function () {
            document.getElementById('editUserPagesForm').reset();
            modalUpdateUserInitialize.hide();
            modalUserPagesInitialize.show();
            getUserAccessPages();
        });

        function getUserAccessPages() {
            document.getElementById('editUserPagesForm').reset();
            functionFetch('/Pages/GetAllUserAndAccessPages/?idUser=' + userId, {}, 'GET', successGetUserAccessPages);
        }

        function successGetUserAccessPages(data) {
            if (data.success) {

                var container = document.getElementById("containerUserPagesCheck");
                container.innerHTML = '';

                data.data.forEach(function (element) {
                    var div = document.createElement("div");
                    div.classList.add("form-check", "form-switch");

                    var input = document.createElement("input");
                    input.classList.add("form-check-input");
                    input.type = "checkbox";
                    input.id = element.id;
                    input.checked = element.userAccess;

                    var label = document.createElement("label");
                    label.classList.add("form-check-label");
                    label.htmlFor = element.id;
                    label.textContent = element.title;

                    div.appendChild(input);
                    div.appendChild(label);
                    container.appendChild(div);
                });
            }
            else {
                alertAnimatedCustom(data.message, 'error', 'An error occurred');
            }
        }
        

    });    
}









