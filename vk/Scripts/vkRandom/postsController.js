var app = angular.module('randomMessages', ['ui.bootstrap', 'ngAnimate', 'duScroll']);

app.controller('postsController', function ($scope, $http, $document) {

    $scope.loadingGlyphClass = "hidden";

    $scope.getVkNames = function (posts) {

        console.log('Fetching vk names');

        args = {
            ids: posts.map(function (value) {
                return value.OwnerId;
            })
        };

        $http.post('api/users', args).success(function (response) {
            console.log(response);
            response.forEach(function (user) {
                posts.forEach(function (post) {
                    if (post.OwnerId == user.Id) {
                        post.displayName = user.FirstName + ' ' + user.LastName;
                        post.avatarURL = user.Photo;
                    }
                });
            });

            // запилить на случай фэйла остановку анимации загрузки
        }).then(function(){
            $scope.loadingGlyphClass = "hidden";
        });
    }

    $scope.load = function () {
        $scope.loadingGlyphClass = "loading";
        $http.get("api/posts").success(function (response) {
            var receivedPosts = response.map(function (value) {
                value.displayName = 'id' + value.OwnerId;
                value.avatarURL = "/Content/img/placeholder.png";
                return value;
            });
            if ($scope.posts)
                $scope.posts = receivedPosts.concat($scope.posts);
            else
                $scope.posts = receivedPosts;
            $scope.getVkNames(receivedPosts);
            console.log($scope.posts)
        });
    }


    $scope.remove = function (post) {
        $scope.posts.splice($scope.posts.indexOf(post), 1);
    }

    $scope.clear = function () {
        $scope.posts.splice(0, $scope.posts.length);
    }



    $scope.lastPosition = 0;
    var trackPosition = true;
    $scope.scrollerClick = function () {
        var pos = $document.scrollTop();
        trackPosition = false;
        $document.scrollTopAnimated($scope.lastPosition, 1).then(function () {
            trackPosition = true;
            $scope.lastPosition = pos;
        });
        $scope.lastPosition = pos;
    }

    $document.on('scroll', function () {
        if (trackPosition)
            if ($scope.lastPosition)
                $scope.lastPosition = 0;
        console.log($document.scrollTop());
    });


    $scope.scrollThingMouseOver = function () {
        $scope.scrollThing = 'scrollThing.hover';
    }


    $scope.load();
});