require 'json'
require 'open-uri'
require 'capybara'
require 'capybara/selenium/driver'
require 'securerandom'
OpenSSL::SSL::VERIFY_PEER = OpenSSL::SSL::VERIFY_NONE
DEV_BY = 'https://dev.by/registration'
@key = '';link = ''; kol1 = 0
web_process = Capybara::Session.new(:selenium)
ARGV[0].to_i.times do
  while true do
     #Generating random username and password
     random_User=SecureRandom.hex(4)
     random_Password=SecureRandom.urlsafe_base64(8)
     #Getting a random email and storing it in a variable
     response = JSON.parse((open("https://post-shift.ru/api.php?action=new&type=json")).string)
     if response['key']==nil then exit end
     email = response['email']
     @key = response['key']
     #Process of registration itself
     web_process.visit DEV_BY
     web_process.fill_in('user[username]', :with => random_User)
     web_process.fill_in('user[email]', :with => email)
     web_process.fill_in('user[password]', :with => random_Password)
     web_process.fill_in('user[password_confirmation]', :with => random_Password)
     web_process.check ('user_agreement')
     web_process.find('input.btn[type="submit"').click
     #Getting a first letter from email
     while true do begin
       response=JSON.parse((open("https://post-shift.ru/api.php?action=getmail&type=json&key=#{@key}&id=1")).string)
         redo if response['message'] == nil
         link=response['message'].split('<')[1].chop.split('=')
         link[1].slice!(0..1)
         web_process.visit link.join('=')
         break
     end end
     #Looking for troubles or errors
     if web_process.first('.block-alerts') == nil
       response=JSON.parse((open("https://post-shift.ru/api.php?action=delete&type=json&key=#{@key}")).string)
       print response['delete']
       kol1 += 1
       break
     end
     #Deleting the email and printing the results
     response=JSON.parse((open("https://post-shift.ru/api.php?action=delete&type=json&key=#{@key}")).string)
     print response['delete']
     puts "\tlogin:#{random_User}\t\tpass:#{random_Password}\t\n"
     break end end
     print "Unsuccessful registrations are equals to: #{kol1}"