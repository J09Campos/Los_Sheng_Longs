$(document).ready(function () {
            var modalMessage = "@(TempData["ModalMessage"] ?? "")";
            if (modalMessage.length > 0) {
        $("#systemMessageModal").modal("show");
            }
        });
