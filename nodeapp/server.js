// =======================
// get the packages we need ============
// =======================

var express     = require('express');
var app         = express();
var bodyParser  = require('body-parser');
var morgan      = require('morgan');
var mongoose    = require('mongoose');

var jwt    = require('jsonwebtoken'); // used to create, sign, and verify tokens
var config = require('./config'); // get our config file
var User   = require('./app/models/user'); // get our mongoose model
    
// =======================
// configuration =========
// =======================
var port = process.env.PORT || 8079; // used to create, sign, and verify tokens
mongoose.connect(config.database); // connect to database
app.set('superSecret', config.secret); // secret variable

// use body parser so we can get info from POST and/or URL parameters
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

// use morgan to log requests to the console
app.use(morgan('dev'));

var io = require('socket.io').listen(app.listen(port));
//app.listen(port);
console.log('Magic happens at http://localhost:' + port);
// API ROUTES -------------------

// get an instance of the router for api routes
var apiRoutes = express.Router(); 

apiRoutes.post('/createuser', function(req, res) {

  // create a sample user
  var user = new User({ 
	firstName:req.body.firstName,
	lastName:req.body.lastName,
    userName: req.body.userName, 
    password: req.body.password
  });

  // save the sample user
  user.save(function(err) {
    if (err) res.json({ success: false, message:err });

    console.log('User saved successfully');
    res.json({ success: true });
  });
});

// route to authenticate a user (POST http://localhost:8080/api/authenticate)
apiRoutes.post('/authenticate', function(req, res) {
  // find the user
  User.findOne({
    userName: req.body.userName
  }, function(err, user) {

    if (err) res.json({ success: false, message:err });

    if (!user) {
      res.json({ success: false, message: 'Authentication failed. User not found.' });
    } else if (user) {

      // check if password matches
      if (user.password != req.body.password) {
        res.json({ success: false, message: 'Authentication failed. Wrong password.' });
      } else {

        // if user is found and password is right
        // create a token
		var tokenPayload={};
		tokenPayload.userName=user.userName;
        var token = jwt.sign(tokenPayload, app.get('superSecret'), {
          expiresIn: '12h' // expires in 24 hours
        });

        // return the information including token as JSON
        res.json({
          success: true,
          message: 'Success',
          token: token
        });
      }   

    }

  });
});


// route middleware to verify a token
apiRoutes.use(function(req, res, next) {

  // check header or url parameters or post parameters for token
  var token = req.body.token || req.query.token || req.headers['x-access-token'];

  // decode token
  if (token) {

    // verifies secret and checks exp
    jwt.verify(token, app.get('superSecret'), function(err, decoded) {      
      if (err) {
        return res.json({ success: false, message: 'Failed to authenticate token.' });    
      } else {
        // if everything is good, save to request for use in other routes
        req.decoded = decoded;    
        next();
      }
    });

  } else {

    // if there is no token
    // return an error
    res.json({ success: false, message: 'Failed to authenticate token.' });
    
  }
});

 apiRoutes.get('/', function(req, res) {
  res.json({ message: 'Welcome to the coolest API on earth!' });
});

apiRoutes.post('/updatecoordinates', function(req, res) {
  User.findOne({userName : req.body.userName}, function(err, user) {
  
	if (err) res.json({ success: false, message:err });
  
	user.longitude= req.body.longitude;
	user.latitude= req.body.latitude;

	user.save(function(err) {
		if (err) throw err;

		console.log('Coordinate saved successfully');
		io.emit('coordinate_changed', "Changed");
		res.json({ success: true });
	});
	
  });
});   

apiRoutes.get('/usercoordinates', function(req, res) {

  
  User.find({}, function(err, users) {
  
	if (err) res.json({ success: false, message:err });
	
	var usersDatas=new Array();
	users.forEach(function(user){
		var userData = {};
		userData.firstName = user.firstName;
		userData.lastName = user.lastName;
		userData.longitude = user.longitude;
		userData.latitude = user.latitude;
		usersDatas.push(userData);
		 
	});  
    res.json({ userCoordinates: usersDatas, success: true });
  });
  
});   

// apply the routes to our application with the prefix /api
app.use('/api', apiRoutes);



/* io.on('connection', function (socket) {
  socket.on('hi', function(msg){
    io.emit('hinew', msg);
  });
}); */