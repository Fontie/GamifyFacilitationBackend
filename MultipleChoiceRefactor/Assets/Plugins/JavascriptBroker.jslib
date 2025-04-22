mergeInto(LibraryManager.library, {
    SendScoreToBackEnd: function(score, gameName) {
        if (typeof window.SendScoreToBackEnd === "function") {
            window.SendScoreToBackEnd(score, UTF8ToString(gameName));
        } else {
            console.error("Error: SendScoreToBackEnd is not defined on window.");
        }
    },
});
