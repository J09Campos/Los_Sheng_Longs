
        // cuando se abre el modal, tomar data-id y data-nombre del botón
        var deleteModal = document.getElementById('deleteModal');
        deleteModal.addEventListener('show.bs.modal', function (event) {
            var button = event.relatedTarget;
        var id = button.getAttribute('data-id');
        var nombre = button.getAttribute('data-nombre');

        document.getElementById('idArea').value = id;
        document.getElementById('nombreArea').textContent = nombre;
        });


