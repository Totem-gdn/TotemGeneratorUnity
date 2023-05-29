var plugin = {
  popUpWindow: null,

  OpenURLPopup: function (url) {
    url = UTF8ToString(url);
    popUpWindow = window.open(url, "Totem Login", "width=600,height=700");
  },

  ClosePopup: function () {
    if (popUpWindow != null) {
      popUpWindow.close();
    }
  },
};
mergeInto(LibraryManager.library, plugin);
