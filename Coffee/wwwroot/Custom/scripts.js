
// Ajax list account
$(document).ready(function () {
    $('#table').DataTable({
        severSide: false,
        responsive: true,
        processing: true,
        scrollY: true,
        scrollX: true,
        ajax:
        {
            url: "/Accounts/JsonAccount",
            dataSrc: '',
            datatype: "json"
        },
        columns: [
            { data: 'username' },
            {
                data: 'role',
                render: function (role) {
                    if (role == "Administrator") return "Quản trị viên";
                    else if (role == "Staff") return "Nhân viên";
                    else return "Khách hàng";
                }
            },
            {
                data: 'status',
                render: function (status) {
                    if (status == true) {
                        return `<div class='text-center'>
                                            <span class='badge badge-pill badge-cyan'>Hoạt động</span>
                                        </div>`;

                    }
                    else {
                        return `<div class='text-center'>
                                            <span class='badge badge-pill badge-red'>Tạm khóa</span>
                                       </div>`;
                    }
                }
            },
            {
                data: 'id',
                render: function (data) {
                    return `<div class='text-center'>
                                          <a class ='btn btn-success text-white' href='/Accounts/Detail?id=${data}'><i class="anticon anticon-edit"></i> Chi tiết</a>
                                          <a class ='btn btn-danger text-white btn-accept' href='/Accounts/reset' data-account-id="${data}" data-toggle="modal" data-target="#notification-modal" ><i class="anticon anticon-undo"></i> Đặt lại mật khẩu</a>
                          </div>`;


                }
            }
        ],
        "fnDrawCallback": function (oSettings) {
            const btnAccepts = document.querySelectorAll('.btn-accept')
            btnAccepts.forEach(btn => {
                btn.onclick = (e) => {
                    e.preventDefault();
                    let idAccount = e.target.getAttribute('data-account-id');
                    let action = e.target.getAttribute('href');
                    let modal = e.target.getAttribute('data-target');

                    document.querySelector(modal).querySelector('.form-accept').setAttribute('action', action);
                    document.querySelector(modal).querySelector('.form-accept input[name=Id]').value = idAccount;
                }
            })
        }
    });

});
// Popup notification reset password
function Open() {
    var myDiv = document.getElementById("#notification-body");
    myDiv.innerHTML = message;
}
/*$('#notification-modal').modal('Show');*/