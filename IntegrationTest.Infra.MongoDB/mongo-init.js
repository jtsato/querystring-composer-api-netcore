print('Start #################################################################');

db = new Mongo().getDB("querystring-mongodb");

db.createUser({
    user: 'xunit',
    pwd: 'xunit',
    roles: [
        {
            role: 'readWrite',
            db: 'querystring-mongodb',
        },
    ],
});

db.createCollection('query_structures', {capped: false});
db.createCollection('query_structures_sequences', {capped: false});

print('End #################################################################');
