var app = angular.module('randomMessages', []);

app.controller('postsController', function ($scope, $http) {
    $scope.getVkNames = function () {
        console.log('Fetching vk names')
        args = {
            ids: $scope.posts.map(function (value) {
                return value.OwnerId;
            }
            )
        };
        fakeArgs = {
            ids: [1,2,3,4]
        };
        console.log(fakeArgs);
        $http.post('api/users', fakeArgs)
        .success(function (response) {
            console.log(response);
        });
    }
    $scope.load = function () {
        $http.get("api/posts")
        .success(function (response) {
            $scope.posts = response.map(function (value) {
                value.displayName = 'id' + value.OwnerId;
                return value;
            });
            $scope.getVkNames();
        });
    }


    $scope.load();
});