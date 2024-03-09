//document.addEventListener("DOMContentLoaded", function () {

//});

if ($('#editUserPagesForm').length) {
    $(document).ready(function () {        
        
        document.getElementById("btnUpdateUserPages").addEventListener("click", function () {

            var lstChecks = getUserPagesChecks();
            functionFetch('/Pages/UpdateUserPages/', lstChecks, 'PUT', successUpdateUserAccessPages);

        });

        function successUpdateUserAccessPages(data) {
            if (data.success) {
                modalUserPagesInitialize.hide();
                alertAnimatedCustom(data.message, 'success', 'successfully updated');
            }
            else {
                alertAnimatedCustom(data.message, 'error', 'An error occurred');
            }
        }

        function getUserPagesChecks() {
            var contianer = document.getElementById("containerUserPagesCheck");
            var checkboxes = contianer.querySelectorAll("input[type='checkbox']");
            var values = [];

            checkboxes.forEach(function (checkbox) {

                if (checkbox.checked) {
                    values.push({
                        IdUser: userId,
                        IdScreen: checkbox.id
                    });
                }
            });

            if (values.length === 0) {
                values.push({ IdUser: userId, IdScreen: -1 });
            }

            return values;
        }

    });    
}



