mergeInto(LibraryManager.library, {
    GetDataForUnity: function() {
        if (typeof window.GetDataForUnity === "function") {
            console.log("Calling GetDataForUnity from Unity...");
            window.GetDataForUnity(); // Call the frontend function
        } else {
            console.error("Error: GetDataForUnity is not defined on window.");
        }
    },

    SavePlayerProgress: function(playerName, xx, yy, zz) {

        if (typeof window.SavePlayerProgress === "function") {
            window.SavePlayerProgress(UTF8ToString(playerName), xx,yy,zz);
        } else {
            console.error("Error: SavePlayerProgress is not defined on window.");
        }
    },

    enterLevel: function(playerName, xx, yy, zz, url) {

        if (typeof window.enterLevel === "function") {
            window.enterLevel(UTF8ToString(playerName), xx,yy,zz,UTF8ToString(url));
        } else {
            console.error("Error: enterLevel is not defined on window.");
        }
    },

    OpenInSamePage: function(urlPtr) {
        var url = UTF8ToString(urlPtr);
        if (typeof window.OpenInSamePage === "function") {
            window.OpenInSamePage(url);
        } else {
            console.error("Error: OpenInSamePage is not defined on window.");
        }
    }
});
