
<!-- Following instructions to run the application -->
<!-- You can skip some scripts based on the comment given -->

<!-- Open folder in terminal (All script shpuld be run in this terminal)-->

<!-- stop all running containers in docker -->
docker system prune -f

<!-- Clear the database -->
docker volume prune -f

<!-- Run application -->
docker compose -f docker-compose.yml up

<!-- If you want to close the application just press ctrl+c (Windows) or CMD+c (Mac) -->

<!-- If you close the terminal by mistake, simply open the folder in terminal and run -->
docker compose -f docker-compose.yml down 

<!-- To clear out the database of this application -->
docker compose -f docker-compose.yml down -v

<!-- Credentials  all password = Abcde@12345 -->
<!-- This is the main admin that has all the access to the pages -->
email: admin@mail.com

<!-- You can use this to check the emails of all other user in the application and login with their email and password -->

<!-- Call me if you face any issue -->
