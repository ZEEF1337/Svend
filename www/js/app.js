// Dom7
var $$ = Dom7;

// Framework7 App main instance
var app = new Framework7({
  root: '#app', // App root element
  id: 'io.framework7.testapp', // App bundle ID
  name: 'Framework7', // App name
  theme: 'auto', // Automatic theme detection
  touch: { mdTouchRipple: false },
  // App root data
  data: function () {
    return {
      serverIP: "https://svendapi.test/endpoints/web/", //Endpoint IP
      fornavn: "", //Logged in info
      efternavn: "", //Logged in info
      sessionToken: "", //Token til API
    };
  },
  // App root methods
  methods: {


  },
  // App routes
  routes: routes,

  on: {
    pageInit: function (page) {

      // Dette er for at sikre os at den første side der bliver indlæst af systemet, er login siden
      let currentpage = app.views.main.router.currentPageEl.dataset.name;
      if (currentpage == "home") {
        app.views.main.router.navigate("/login/", {
          reloadCurrent: true, // Sikrer at der kommer friskt data på siden
        });
      };
    },
  },
});

// Init/Create views
var homeView = app.views.create('#view-home', {
  url: '/',
  animate: false,
  main: true,
  master: true,
});