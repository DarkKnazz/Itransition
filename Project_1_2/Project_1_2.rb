require 'json'
require 'open-uri'
require 'capybara'
require 'capybara/selenium/driver'
require 'securerandom'
OpenSSL::SSL::VERIFY_PEER = OpenSSL::SSL::VERIFY_NONE
DEV_BY = 'https://dev.by/registration'
@key = '';link = ''; kol1 = 0
session = Capybara::Session.new(:selenium)
ARGV[0].to_i.times do
  while true do
     username=SecureRandom.hex(4)
     password=SecureRandom.urlsafe_base64(8, false)

     data_hash = JSON.parse((open("https://post-shift.ru/api.php?action=new&type=json")).string)
     if data_hash['key']==nil then exit end
     email = data_hash['email']
     @key = data_hash['key']

     session.visit DEV_BY
     session.fill_in('user[username]', :with => username)
     session.fill_in('user[email]', :with => email)
     session.fill_in('user[password]', :with => password)
     session.fill_in('user[password_confirmation]', :with => password)
     session.check ('user_agreement')
     session.find('input.btn[type="submit"').click

     if session.first('.block-alerts') != nil
       data_hash=JSON.parse((open("https://post-shift.ru/api.php?action=delete&type=json&key=#{@key}")).string)
       print data_hash['delete']
       kol1 += 1
       break
     end

     while true do begin
         data_hash=JSON.parse((open("https://post-shift.ru/api.php?action=getmail&type=json&key=#{@key}&id=1")).string)
         redo if data_hash['message'] == nil
         link=data_hash['message'].split('<')[1].chop.split('=')
         link[1].slice!(0..1)
         session.visit link.join('=')
         break
     end end

     data_hash=JSON.parse((open("https://post-shift.ru/api.php?action=delete&type=json&key=#{@key}")).string)
     print data_hash['delete']
     puts "\tlogin:#{username}\t\tpass:#{password}\t\n"
     break end end
     print "Unsuccessful registrations are equals to: #{kol1}"