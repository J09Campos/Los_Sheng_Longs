
        $(document).ready(function () {
            $('.table').DataTable({
                "pageLength": 10,
                "lengthChange": false, // oculta selector de "mostrar 10, 25, etc."
                "language": {
                    "paginate": {
                        "previous": "Anterior",
                        "next": "Siguiente"
                    },
                    "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                    "zeroRecords": "No se encontraron resultados"
                }
            });
        });
 
