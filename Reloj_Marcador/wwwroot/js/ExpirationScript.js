let tiempoInactividad = 300000;

let timer = setTimeout(mostrarModal, tiempoInactividad);

// Reinicia el timer cuando hay actividad

window.onload = resetTimer;
document.onmousemove = resetTimer;
document.jsonkeypress = resetTimer;
document.onclick = resetTimer;

function resetTimer() {
    clearTimeout(timer);
    timer = setTimeout(mostrarModal, tiempoInactividad);
}

function mostrarModal() {

    $('#sessionExpiredModal').modal('show');
}


document.getElementById('redirectLoginBtn').addEventListener('click', function () {
    window.location.href = '/Login/Login?expired=true';
});