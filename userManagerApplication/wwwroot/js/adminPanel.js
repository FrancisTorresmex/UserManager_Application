

if ($('#updateUserModal').length) {
    $(document).ready(function () {

        createTableUsers();

        let modalUserPages = document.getElementById('userPagesModal');
        modalUserPagesInitialize = new bootstrap.Modal(modalUserPages);

        let modalUpdateUser = document.getElementById('updateUserModal');
        modalUpdateUserInitialize = new bootstrap.Modal(modalUpdateUser);

        resetColorField('requiredCreateUser');


        //open modal addUser
        document.getElementById("btnCreateUser").addEventListener('click', function () {
            userId = 0;
            modalUpdateUserInitialize.show();
            getAllRoles();
            document.getElementById('editUserForm').reset();
        });


    });    
}

function openModalUpdateUser(idUser) {
    modalUpdateUserInitialize.show();    
    getUserData(idUser);
    getAllRoles();
}

function getAllRoles() {
    functionFetch('/Roles/GetAllRoles/', {}, 'GET', successGetAllRoles);
}

function successGetAllRoles(data) {
    if (data.success) {
        let selectRoles = document.getElementById("selRoleUser");
        selectRoles.innerHTML = ''; //empty select

        for (var i = 0; i < data.data.length; i++) {
            var option = document.createElement('option');
            option.value = data.data[i].idRole;
            option.text = data.data[i].name;
            selectRoles.appendChild(option);
        }

    }
    else
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
}



function createTableUsers() {
    tblUsers = $("#userTable").DataTable({        
        "paging": true,
        "destroy": true,
        "order": [[0, 'asc'], [1, 'asc']],
        "searching": true,
        "lengthChange": true,
        "info": true,
        "responsive": true,
        "autoWidth": false,
        "processing": true,
        columns: [
            { data: 'IdUser' },
            { data: 'Name' },
            { data: 'LastName' },
            { data: 'Email' },
            { data: 'Phone' },
            { data: 'RoleName' },
            { data: 'DateAdmision' },
            { data: 'InactiveDate' },
            { data: 'StatusName' },
            {
                data: null,
                render: function (data, type, full, meta) {
                    var userId = data.IdUser;
                    var userStatus = data.statusName;
                    return '<button type="button" class="btn btn-primary" data-id="' + userId + '" id="btnUpdateUser" onclick="openModalUpdateUser(' + userId + ')">Edit</button>' +
                        '<button class="btn btn-info" data-id="' + userId + '"  id="btnInacivateUser' + userId + '" onclick="inactiveActiveUser(\'' + userId + '\', \'' + userStatus + '\')">' + (data.StatusName == "Inactive" ? "Active" : "Inactive") + '</button>';
                }
            }
        ]
    });
}

function getUserData(idUser) {

    userId = idUser;
    functionFetch("/Admin/GetUserData/?id=" + userId, {}, "GET", successGetUserData);
}

function successGetUserData(data) {
    if (data.success) {

        var userData = data.data;
        var userForm = document.getElementById("editUserForm");

        userForm.elements["Name"].value = userData.name;
        userForm.elements["LastName"].value = userData.lastName;
        userForm.elements["Phone"].value = userData.phone;
        userForm.elements["Email"].value = userData.email;
        userForm.elements["Role"].value = userData.idRole;
        userForm.elements["Password"].value = userData.password;
        
    }
    else {
        
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}

//Inactive user
function inactiveActiveUser(idUser, status) {

    idUserTemp = idUser;
    let contraryStatus = status == 'Active' ? 'Inactive' : 'Active';

    var model = {
        IdUser: idUser,
        Status: contraryStatus
    };

    functionFetch("/Admin/InactiveUser/", model, "PUT", successInactiveActiveUser);
}

function successInactiveActiveUser(data) {

    if (data.success) {

        var row = tblUsers.row('tr[data-id="' + idUserTemp + '"]');
        var rowData = row.data();

        var btn = document.getElementById("btnInacivateUser" + idUserTemp);
        btn.textContent = rowData.StatusName === "Inactive" ? "Active" : "Inactive";

        var status = (rowData.StatusName === "Inactive" ? "Active" : "Inactive");
        var objRows = {
            StatusName: status
        };
        updateRowDataTable(tblUsers, idUserTemp, objRows, false);
        
    }
    else
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
}

//update user
function updateUser() {

    //form fields
    var editUserForm = document.getElementById("editUserForm");

    var name = editUserForm.elements["Name"].value;
    var lastName = editUserForm.elements["LastName"].value;
    var phone = editUserForm.elements["Phone"].value;
    var email = editUserForm.elements["Email"].value;
    var role = editUserForm.elements["Role"].value;
    var password = editUserForm.elements["Password"].value;

    if (Validate('.requiredCreateUser').length === 0) {
        var user = {
            IdUser: userId,
            Name: name,
            LastName: lastName,
            Phone: phone,
            Email: email,
            IdRole: role,
            Password: password
        };

        if (userId == 0) //add user
            addUserData(user);
        else
            updateUserData(user);
    }

}

function updateUserData(user) {
    functionFetch('/Admin/UpdateUser/', user, 'PUT', successUpdateUserData);
}

function successUpdateUserData(data) {

    if (data.success) {

        modalUpdateUserInitialize.hide();
        alertAnimatedCustom(data.message, 'success', 'User updated');

        var dataUserUpdated = data.data;

        var objUser = {
            IdUser: dataUserUpdated.idUser,
            LastName: dataUserUpdated.lastName,
            Name: dataUserUpdated.name,
            Email: dataUserUpdated.email,
            Phone: dataUserUpdated.phone,
            RoleName: dataUserUpdated.roleName,
            DateAdmision: dataUserUpdated.dateAdmision,
            InactiveDate: dataUserUpdated.inactiveDate,
            StatusName: dataUserUpdated.statusName,
        };

        updateRowDataTable(tblUsers, dataUserUpdated.idUser, objUser, true);
    }
    else {
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}

//Add new user
function addUserData(user) {
    functionFetch('/Admin/AddUser/', user, 'POST', successAddUserData);
}

function successAddUserData(data) {
    if (data.success) {

        modalUpdateUserInitialize.hide();
        alertAnimatedCustom(data.message, 'success', 'User created successfully');

        var dataUserCreated = data.data;

        //get rol name local
        let rolTemp = document.getElementById("selRoleUser");
        let selectedIndex = rolTemp.selectedIndex;
        let rolrolNameTemp = rolTemp.options[selectedIndex].text;;

        var objUser = {
            IdUser: dataUserCreated.idUser,
            LastName: dataUserCreated.lastName,
            Name: dataUserCreated.name,
            Email: dataUserCreated.email,
            Phone: dataUserCreated.phone,
            RoleName: rolrolNameTemp,
            DateAdmision: dataUserCreated.dateAdmision,
            InactiveDate: dataUserCreated.inactiveDate,
            StatusName: dataUserCreated.status,
        };

        tblUsers.row.add(objUser).draw(false);
    }
    else {
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}


