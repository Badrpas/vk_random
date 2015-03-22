var app = angular.module('randomMessages', []);

app.controller('postsController', function ($scope, $http) {
    $scope.getVkNames = function () {
        console.log('Fetching vk names');
        args = {
            ids: $scope.posts.map(function (value) {
                return value.OwnerId;
            }
            )
        };

        $http.post('api/users', args).success(function (response) {
            console.log((response));
            for (bar of response)
                console.log(bar);
        });
    }
    $scope.load = function () {
        $http.get("api/posts").success(function (response) {
            $scope.posts = response.map(function (value) {
                value.displayName = 'id' + value.OwnerId;
                return value;
            });
            $scope.getVkNames();
        });
    }


    $scope.load();
});