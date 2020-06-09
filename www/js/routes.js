routes = [
  {
    path: '/',
    url: './index.html',
  },
  {
    name: 'login',
    path: '/login/',
    componentUrl: './pages/login.html',
    on:{
      pageInit: function (event, page){
      }
    }
  },
  {
    name: 'register',
    path: '/register/',
    componentUrl: './pages/register.html',
    on:{
      pageInit: function (event, page){
      }
    }
  },
  {
    name: 'overview',
    path: '/overview/',
    componentUrl: './pages/overview.html',
    on:{
      pageInit: function (event, page){
      }
    }
  },
  {
    name: 'newticket',
    path: '/newticket/',
    componentUrl: './pages/newticket.html',
    on:{
      pageInit: function (event, page){
      }
    }
  },
  // Default route (404 page). MUST BE THE LAST
  {
    name: 'notfound',
    path: '(.*)',
    componentUrl: './pages/404.html',
    'data-view': 'view-main',
  },
];
