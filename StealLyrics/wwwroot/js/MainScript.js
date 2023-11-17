function ProcurarMusica() {
    document.getElementById("letramusica").innerHTML = "";

    var nome = document.getElementById("nomeMsc").value;

    if (nome != "") {
        document.getElementById("loadingGif").style.display = "block";
        $.ajax({
            url: "/Home/GetMusica",
            data: JSON.stringify(nome),
            type: "POST",
            contentType: 'application/json',
            success: function (data) {
                document.getElementById("loadingGif").style.display = "none";
                procurarMusicaCallback(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById("loadingGif").style.display = "none";
                console.error("Erro na requisição: " + textStatus + " - " + errorThrown);
            }
        });
    }
}

function procurarMusicaCallback(data) {
    if (data) {
        document.getElementById("letramusica").innerHTML = data;
        document.getElementById("form-insira-musica").style.display = "none";
    }
}

function fecharVideo() {
    document.getElementById("videoContainer").setAttribute("hidden", "hidden");
}